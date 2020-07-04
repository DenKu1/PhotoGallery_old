import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Album } from '../../models/album';
import { Photo } from '../../models/photo';
import { User } from '../../models/user';
import { AlbumService } from '../../services/album.service';
import { PhotoService } from '../../services/photo.service';
import { CommentService } from '../../services/comment.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-photo',
  templateUrl: './photo.component.html',
  styleUrls: ['./photo.component.css']
})
export class PhotoComponent implements OnInit {
  isOwned: boolean;
  albumId: number;

  currentUser: User;
  album: Album;

  photos: Photo[];

  crInfo: CreatePhotoInfo;
  upInfo: UpdatePhotoInfo;

  crCommentInfos: CreateCommentInfo[];

  searchPhotoForm: FormControl;

  hideme: any = {};

  constructor(
    private activeRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private albumService: AlbumService,
    private photoService: PhotoService,
    private commentService: CommentService,
    private userService: UserService) {

    this.activeRoute.params.subscribe(routeParams => this.albumId = routeParams.id);

    this.searchPhotoForm = new FormControl('', [Validators.maxLength(50)])

    this.crInfo = new CreatePhotoInfo(this.formBuilder, this.albumId);
    this.upInfo = new UpdatePhotoInfo(this.formBuilder);
  }

  ngOnInit(): void {
    
    
    this.getCurrentUser();
    this.getAlbum();
    this.getPhotos();
  }

  getCurrentUser(): void {
      this.userService.currentUser.subscribe(user => this.currentUser = user);
  }

  getAlbum(): void {
    this.albumService.getAlbum(this.albumId)
      .pipe(first())
      .subscribe(album =>
      {
        this.album = album;
        this.isOwned = album.userId === this.currentUser.id;
      });
  }

  getPhotos(): void {
    this.photoService.getPhotos(this.albumId)
      .pipe(first())
      .subscribe(
        photos =>
        {
          this.photos = photos;
          this.crCommentInfos = photos.map(p => new CreateCommentInfo(this.formBuilder, p.id));
        });
  }

  createPhoto(): void {
    this.crInfo.submitted = true;
    this.crInfo.success = '';
    this.crInfo.error = '';

    if (this.crInfo.form.invalid) {
      return;
    }

    this.crInfo.loading = true;
    this.photoService.createPhoto(
      +this.crInfo.f.albumId.value,
      this.crInfo.f.name.value,
      this.crInfo.f.path.value)
      .pipe(first())
      .subscribe(
        photo => {
          this.photos.push(photo);
          this.crCommentInfos.push(new CreateCommentInfo(this.formBuilder, photo.id));

          this.crInfo.success = "Created successfully";
          this.crInfo.form.markAsUntouched();

          this.crInfo.loading = false;
        },
        err => {
          this.crInfo.error = "Unknown error! Please try again";
          this.crInfo.loading = false;
        });
  }

  updatePhoto(): void {
    this.upInfo.submitted = true;
    this.upInfo.success = '';
    this.upInfo.error = '';

    if (this.upInfo.form.invalid) {
      return;
    }

    this.upInfo.loading = true;
    this.photoService.updatePhoto(
      this.upInfo.f.id.value,
      this.upInfo.f.name.value)
      .pipe(first())
      .subscribe(
        () => {
          let currentPhoto = this.photos.find(p => p.id === this.upInfo.f.id.value);

          currentPhoto.name = this.upInfo.f.name.value;        

          this.upInfo.success = "Updated successfully";
          this.upInfo.form.markAsUntouched();

          this.upInfo.loading = false;
        },
        err => {
          this.upInfo.error = "Unknown error! Please try again";
          this.upInfo.loading = false;
        });
  }

  deletePhoto(photoId: number, index: number): void {
    this.photoService.deletePhoto(photoId)
      .pipe(first())
      .subscribe(
        () => {
          this.photos.splice(index, 1);
          this.crCommentInfos.splice(index, 1);
        },
        err => {
          console.log("Can`t delete photo! Unknown error");
        });
  }

  likePhoto(photo: Photo): void {
    this.photoService.likePhoto(photo.id)
      .pipe(first())
      .subscribe(
        () => {
          
          if (photo.isLiked) {
            photo.likes--;
          }
          else
          {
            photo.likes++;
          }
          photo.isLiked = !photo.isLiked;
        },
        err => {
          console.log("Can`t like photo! Unknown error");
        });
  }

  searchPhoto() {

    if (this.searchPhotoForm.invalid) {
      return;
    }

    let searchString: string = this.searchPhotoForm.value;

    if (searchString === '') {
      this.hideme = {};
      return;
    }

    this.photos.forEach(
      photo => photo.name.toLowerCase().includes(searchString.toLowerCase())
        ? this.hideme[photo.id] = false
        : this.hideme[photo.id] = true);
  }

  isCommentOwned(commentUserId: number): boolean
  {
    return commentUserId === this.currentUser.id;
  }

  getComments(photo: Photo): void
  {
    this.commentService.getComments(photo.id)
      .pipe(first())
      .subscribe(comments => { photo.comments = comments; });
  }

  createComment(photo: Photo, i: number): void {
    let info: CreateCommentInfo = this.crCommentInfos[i];

    info.submitted = true;
    info.error = '';

    if (info.form.invalid) {
      return;
    }

    info.loading = true;
    this.commentService.createComment(
      info.f.photoId.value,
      info.f.text.value
      )
      .pipe(first())
      .subscribe(
        comment => {
          if (this.photos[i].comments) {
            this.photos[i].comments.push(comment);            
          }
          else
          {
            this.getComments(this.photos[i]);
          }
          info.loading = false;
        },
        err => {
          info.error = "Unknown error! Please try again";
          info.loading = false;
        });
  }

  deleteComment(commentId: number, photoIndex: number, commentIndex: number): void {
    this.commentService.deleteComment(commentId)
      .pipe(first())
      .subscribe(
        () => {
          this.photos[photoIndex].comments.splice(commentIndex, 1);
        },
        err => {
          console.log("Can`t delete comment! Unknown error");
        });
  }

}

class CreatePhotoInfo {
  loading = false;
  submitted = false;
  error: string = '';
  success: string = '';

  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder, albumId: number) {
    this.form = this.formBuilder.group({
      albumId: [albumId, [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      path: ['', [Validators.required, Validators.maxLength(200)]]
    });
  }
}

class UpdatePhotoInfo {
  loading = false;
  submitted = false;
  error: string = '';
  success: string = '';

  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      id: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(50)]]     
    });
  }

  initialize(photo: Photo): void {
    this.form.setValue({
      id: photo.id,
      name: photo.name
    });
    this.form.markAsUntouched();
  }
}

class CreateCommentInfo {
  loading = false;
  submitted = false;
  error: string = '';

  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder, photoId: number) {
    this.form = this.formBuilder.group({
      photoId: [photoId, [Validators.required]],
      text: ['', [Validators.required, Validators.maxLength(200)]]
    });
  }
}
