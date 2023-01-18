import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { Photo } from '../models/photo';
import { PhotoRecommendations } from '../models/photoRecommendations';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

  constructor(private http: HttpClient)
  {
  }

  getPhotos(albumId: number): Observable<Photo[]> {
    return this.http.get<Photo[]>(`${environment.apiUrl}/albums/${albumId}/photos`);
  }

  getPhotosByTags(tags: string[]): Observable<Photo[]> {
    let params = new HttpParams();
    for (let tag of tags) {
      params = params.append('tags', tag);
    }

    return this.http.get<Photo[]>(`${environment.apiUrl}/photosByTags`, {params: params});
  }

  getPhoto(id: number): Observable<Photo> {
    return this.http.get<Photo>(`${environment.apiUrl}/photos/${id}`)
  }

  getPhotoRecommendations(id: number): Observable<PhotoRecommendations> {
    return this.http.get<PhotoRecommendations>(`${environment.apiUrl}/photoRecommendations/${id}`)
  }

  createPhoto(albumId: number, name: string, path: string): Observable<Photo> {
    return this.http.post<Photo>(`${environment.apiUrl}/albums/${albumId}/photos`, { name, path })
  }

  updatePhoto(id: number, name: string): Observable<Photo> {
    return this.http.put<Photo>(`${environment.apiUrl}/photos/${id}`, { name })
  }

  deletePhoto(id: number) {
    return this.http.delete(`${environment.apiUrl}/photos/${id}`)
  }

  likePhoto(id: number) {
    return this.http.post(`${environment.apiUrl}/photos/${id}/like`, {})
  }

  attachPhotoTags(id: number, tags: string[]) {
    return this.http.post(`${environment.apiUrl}/photos/${id}/attachTags`, tags,{headers: this.headers})
  }

  detachPhotoTag(id: number, tag: string) {
    return this.http.post(`${environment.apiUrl}/photos/${id}/detachTag`, `"${tag}"`,{headers: this.headers})
  }
}

