import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { first } from 'rxjs/operators';
import {ActivatedRoute} from "@angular/router";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AlbumService} from "../../services/album.service";
import {PhotoService} from "../../services/photo.service";
import {CommentService} from "../../services/comment.service";
import {Photo} from "../../models/photo";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  currentUser: User;

  crTagInfo: CreateTagInfo;

  constructor(
    private activeRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private userService: UserService) {
  }

  ngOnInit() {
    this.getCurrentUser();

    this.crTagInfo = new CreateTagInfo(this.formBuilder, this.currentUser.id)
  }

  getCurrentUser(): void {
    this.userService.currentUser.subscribe(user => this.currentUser = user);
  }

  attachUserTag(): void {
    this.crTagInfo.submitted = true;
    this.crTagInfo.error = '';

    if (this.crTagInfo.form.invalid) {
      return;
    }

    this.crTagInfo.loading = true;
    this.userService.attachUserTags(
      [this.crTagInfo.f.name.value]
    )
      .pipe(first())
      .subscribe(
        x => {
          if (!this.currentUser.tags.includes(this.crTagInfo.f.name.value)){
            this.currentUser.tags.push(this.crTagInfo.f.name.value);
          }

          this.crTagInfo.loading = false;
        },
        err => {
          this.crTagInfo.error = "Unknown error! Please try again";
          this.crTagInfo.loading = false;
        });
  }

  detachUserTag(tag: string): void {
    this.userService.detachUserTag(tag)
      .pipe(first())
      .subscribe(
        () => {
          this.currentUser.tags.splice(this.currentUser.tags.indexOf(tag), 1);
        },
        err => {
          console.log("Can`t detach tag! Unknown error");
        });
  }
}

class CreateTagInfo {
  loading = false;
  submitted = false;
  error: string = '';

  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder, userId: number) {
    this.form = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]]
    });
  }
}
