import { Component, OnInit } from '@angular/core';
import { Album } from '../../models/album';
import { AlbumService } from '../../services/album.service';
import { first } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit {

  crInfo: CreateAlbumInfo;  
  
  albums: Album[];

  constructor(
    private formBuilder: FormBuilder,
    private albumService: AlbumService) { }

  ngOnInit(): void {
    this.getAlbums();

    this.crInfo = new CreateAlbumInfo(this.formBuilder);
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
        },
        err => {
          this.crInfo.error = "Unknown error! Please try again";
          this.crInfo.loading = false;
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


  /*
  Remove(album: Album) {
    let album = this.albums..splice(index, 1);
    
    //Вызов сервиса

    //Если не ОК вывести сообщение, вернуть элемент
  }*/

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
