import { Component, OnInit } from '@angular/core';

import { Album } from 'src/app/models/album';
import { AlbumService } from '../../services/album.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-album-get-all',
  templateUrl: './album-get-all.component.html',
  styleUrls: ['./album-get-all.component.css']
})
export class AlbumGetAllComponent implements OnInit {

  albums: Album[];

  constructor(private albumService: AlbumService) { }

  ngOnInit(): void {
    this.albumService.getAlbums()
      .pipe(first())
      .subscribe(albums => { this.albums = albums; });
  }

  Remove(index: number)
  {
    let album = this.albums.splice(index, 1);

    //Вызов сервиса

    //Если не ОК вывести сообщение, вернуть элемент
  }

}
