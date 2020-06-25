import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { Photo } from '../models/photo';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private http: HttpClient)
  {
  }

  getPhotos(albumId: number): Observable<Photo[]> {
    return this.http.get<Photo[]>(`${environment.apiUrl}/albums/${albumId}/photos`);
  }

  getPhoto(id: number): Observable<Photo> {
    return this.http.get<Photo>(`${environment.apiUrl}/photos/${id}`)
  }

  createPhoto(albumId: number, name: string, path: string): Observable<Photo> {
    return this.http.post<Photo>(`${environment.apiUrl}/albums/${albumId}/photos`, { albumId, name, path })
  }

  updatePhoto(id: number, name: string): Observable<Photo> {
    return this.http.put<Photo>(`${environment.apiUrl}/photos/${id}`, { id, name })
  }

  deletePhoto(id: number) {
    return this.http.delete(`${environment.apiUrl}/photos/${id}`)
  }

  likePhoto(id: number) {
    return this.http.post(`${environment.apiUrl}/photos/${id}/like`, {})
  }
}

