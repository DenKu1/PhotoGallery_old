import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserService } from './user.service';
import { Album } from '../models/album';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  currentUser: User;

  constructor(
    private http: HttpClient,
    private userService: UserService
  )
  {
    this.userService.currentUser.subscribe(x => this.currentUser = x);
  }

  getAlbums() {
    return this.http.get<Album[]>(`${environment.apiUrl}/users/${this.currentUser.id}/albums`);
  }
  /*
  createBid(body) {
    return this.http.post(this.rootUrl + '/bids', body);
  }

  getBidsByUser(): Observable<Bid[]> {
    return this.http.get<Bid[]>(this.rootUrl + '/users/profile/bids');
  }

  deleteBid(id) {
    return this.http.delete(this.rootUrl + '/bids/' + id);
  }
  */
  
}

