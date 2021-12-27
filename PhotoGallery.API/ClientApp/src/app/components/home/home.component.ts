import { Component, OnInit } from '@angular/core';
import {Photo} from "../../models/photo";
import {ActivatedRoute} from "@angular/router";
import {PhotoService} from "../../services/photo.service";
import {UserService} from "../../services/user.service";
import {first} from "rxjs/operators";
import {User} from "../../models/user";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentUser: User;
  photos: Photo[];

  constructor(
    private activeRoute: ActivatedRoute,
    private photoService: PhotoService,
    private userService: UserService) { }

  ngOnInit() {
    this.getCurrentUser();
    this.getPhotos();
  }

  getCurrentUser(): void {
    this.userService.currentUser.subscribe(user => this.currentUser = user);
  }

  getPhotos(): void {
    this.photoService.getPhotosByTags(this.currentUser.tags)
      .pipe(first())
      .subscribe(
        photos =>
        {
          this.photos = photos;
        });
  }
}
