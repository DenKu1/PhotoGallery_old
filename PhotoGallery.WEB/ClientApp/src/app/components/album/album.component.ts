import { Component, OnInit } from '@angular/core';
import { Album } from '../../models/album';
import { AlbumService } from '../../services/album.service';
import { first } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit {
  albums: Album[];

  crInfo: CreateAlbumInfo;
  upInfo: UpdateAlbumInfo;

  constructor(private formBuilder: FormBuilder, private albumService: AlbumService)
  {
    this.crInfo = new CreateAlbumInfo(this.formBuilder);
    this.upInfo = new UpdateAlbumInfo(this.formBuilder);
  }

  ngOnInit(): void {
    this.getAlbums();   
  }

  getAlbums()
  {
    this.albumService.getAlbums()
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
          this.crInfo.loading = false;
          this.crInfo.form.markAsUntouched();
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
          let currentAlbum = this.albums.find(a => { return a.id === this.upInfo.f.id.value });

          currentAlbum.name = this.upInfo.f.name.value;
          currentAlbum.description = this.upInfo.f.description.value;

          this.upInfo.success = "Updated successfully";
          this.upInfo.loading = false;
          this.upInfo.form.markAsUntouched();
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
