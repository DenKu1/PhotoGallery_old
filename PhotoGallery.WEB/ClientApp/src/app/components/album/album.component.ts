import { Component, OnInit } from '@angular/core';
import { Album } from '../../models/album';
import { AlbumService } from '../../services/album.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit {

  albums: Album[];

  constructor(private albumService: AlbumService) { }

  ngOnInit(): void {
    this.albumService.getAlbums()
      .pipe(first())
      .subscribe(albums => { this.albums = albums; });
  }

  Remove(index: number) {
    let album = this.albums.splice(index, 1);

    //Вызов сервиса

    //Если не ОК вывести сообщение, вернуть элемент
  }

}
