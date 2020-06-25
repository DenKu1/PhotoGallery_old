import { Component, OnInit } from '@angular/core';
import { Album } from '../../models/album';
import { AlbumService } from '../../services/album.service';
import { first } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit {
  isOwned: boolean;

  currentUser: User;
  albumOwner: User;

  albums: Album[];

  crInfo: CreateAlbumInfo;
  upInfo: UpdateAlbumInfo;

  constructor(
    private activeRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private albumService: AlbumService,
    private userService: UserService) {

    this.crInfo = new CreateAlbumInfo(this.formBuilder);
    this.upInfo = new UpdateAlbumInfo(this.formBuilder);
  }

  ngOnInit(): void {
    this.userService.currentUser.subscribe(user => this.currentUser = user);

    this.activeRoute.params.subscribe(routeParams => {
      this.getAlbums(routeParams.id);
      this.isOwned = this.currentUser.id === +routeParams.id;

      this.userService.getUserById(+routeParams.id).subscribe(
        user => { this.albumOwner = user; })
    });
  }

  getAlbums(id: number)
  {
    this.albumService.getAlbums(id)
      .pipe(first())
      .subscribe(albums => { this.albums = albums; });
  }

  createAlbum()
  {
    this.crInfo.submitted = true;
    this.crInfo.success = '';
    this.crInfo.error = '';

    if (this.crInfo.form.invalid) {
      return;
    }    

    this.crInfo.loading = true;
    this.albumService.createAlbum(
      this.crInfo.f.name.value,
      this.crInfo.f.description.value)
      .pipe(first())
      .subscribe(
        album => {
          this.albums.push(album);

          this.crInfo.success = "Created successfully";  
          this.crInfo.form.markAsUntouched();

          this.crInfo.loading = false;
        },
        err => {
          this.crInfo.error = "Unknown error! Please try again";
          this.crInfo.loading = false;
        });    
  }

  updateAlbum() {
    this.upInfo.submitted = true;
    this.upInfo.success = '';
    this.upInfo.error = '';

    if (this.upInfo.form.invalid) {
      return;
    }

    this.upInfo.loading = true;
    this.albumService.updateAlbum(
      this.upInfo.f.id.value,
      this.upInfo.f.name.value,
      this.upInfo.f.description.value)
      .pipe(first())
      .subscribe(
        () => {
          let currentAlbum = this.albums.find(a => a.id === this.upInfo.f.id.value);

          currentAlbum.name = this.upInfo.f.name.value;
          currentAlbum.description = this.upInfo.f.description.value;

          this.upInfo.success = "Updated successfully";         
          this.upInfo.form.markAsUntouched();

          this.upInfo.loading = false;
        },
        err => {
          this.upInfo.error = "Unknown error! Please try again";
          this.upInfo.loading = false;
        });
  }

  deleteAlbum(albumId: number, index: number)
  {
    this.albumService.deleteAlbum(albumId)
      .pipe(first())
      .subscribe(
        () => {
          this.albums.splice(index, 1);         
        },
        err => {
          console.log("Can`t delete album! Unknown error");        
        });    
  }
}

class CreateAlbumInfo
{
  loading = false;
  submitted = false;
  error: string = '';
  success: string = '';

  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder)
  {
    this.form = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.maxLength(200)]]
    });
  }
}

class UpdateAlbumInfo {
  loading = false;
  submitted = false;
  error: string = '';
  success: string = '';
  
  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder)
  {
    this.form = this.formBuilder.group({
      id: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.maxLength(200)]]
    });
  }

  initialize(album: Album): void {    
    this.form.setValue({
      id: album.id,
      name: album.name,
      description: album.description
    });
    this.form.markAsUntouched();
  }
}
