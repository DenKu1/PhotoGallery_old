import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserService } from './user.service';
import { Album } from '../models/album';
import { User } from '../models/user';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  currentUser: User;

  constructor(
    private http: HttpClient,
    private userService: UserService
  ) {
    this.userService.currentUser.subscribe(x => this.currentUser = x);
  }

  getAlbums(id: number = this.currentUser.id): Observable<Album[]> {
    return this.http.get<Album[]>(`${environment.apiUrl}/users/${id}/albums`);
  }

  getAlbum(id: number = this.currentUser.id): Observable<Album> {
    return this.http.get<Album>(`${environment.apiUrl}/albums/${id}`)
  }

  createAlbum(name: string, description: string): Observable<Album> {
    return this.http.post<Album>(`${environment.apiUrl}/albums`, { userId: this.currentUser.id, name, description })
  }

  updateAlbum(id: number, name: string = null, description: string = null): Observable<Album> {
    if (name === null && description === null) {
      return;
    }

    return this.http.put<Album>(`${environment.apiUrl}/albums/${id}`, { id, name, description })
  }

  deleteAlbum(id: number) {
    return this.http.delete(`${environment.apiUrl}/albums/${id}`)
  }
}

