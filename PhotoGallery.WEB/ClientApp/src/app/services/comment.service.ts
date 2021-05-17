import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { Comment } from '../models/comment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient)
  {
  }

  getComments(photoId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${environment.apiUrl}/photos/${photoId}/comments`);
  }

  getComment(id: number): Observable<Comment> {
    return this.http.get<Comment>(`${environment.apiUrl}/comments/${id}`)
  }

  createComment(photoId: number, text: string): Observable<Comment> {
    return this.http.post<Comment>(`${environment.apiUrl}/photos/${photoId}/comments`, { text })
  }

  deleteComment(id: number) {
    return this.http.delete(`${environment.apiUrl}/comments/${id}`)
  }
}

