import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  loading = false;
  submitted = false;
  error: string = '';

  userName: FormControl;

  currentUser: User;

  constructor(
    private router: Router,
    private userService: UserService
  ) {
    this.userService.currentUser.subscribe(x => this.currentUser = x);
    this.userName = new FormControl('', [Validators.required, Validators.maxLength(50)])
  }

  ngOnInit(): void {
  }

  profile() {
    this.router.navigate(['/profile']);
  }

  logout() {
    this.userService.logout();
    this.router.navigate(['/users/login']);
  }

  findUser()
  {
    this.submitted = true;
    this.error = '';

    if (this.userName.invalid) {
      return;
    }

    this.loading = true;
    this.userService.getUserByUserName(this.userName.value)
      .pipe(first())
      .subscribe(
        user => {
          this.router.navigate(['/users', user.id, 'albums']);
        },
        err => {
          this.error = "User was not found";
          this.loading = false;
        });
  }
}
