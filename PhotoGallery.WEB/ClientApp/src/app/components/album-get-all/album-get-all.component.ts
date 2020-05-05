import { Component, OnInit } from '@angular/core';

import { Album } from 'src/app/models/album';

@Component({
  selector: 'app-album-get-all',
  templateUrl: './album-get-all.component.html',
  styleUrls: ['./album-get-all.component.css']
})
export class AlbumGetAllComponent implements OnInit {

  albums: Album[];

  constructor() { }

  ngOnInit(): void {

    this.albums = [{ Id: 0, Created: null, Description: "lalalal", Name: "LOl", Updated: null }];

  }

  Remove(index: number)
  {
    let album = this.albums.splice(index, 1);

    //Вызов сервиса

    //Если не ОК вывести сообщение, вернуть элемент
  }

}
