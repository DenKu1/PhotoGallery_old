import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { UserService } from 'src/app/services/user.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  error : string = '';

  get f() { return this.loginForm.controls; }

  currentUser: User;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService) {
    // redirect to home if already logged in
    this.userService.currentUser.subscribe(x => this.currentUser = x);
    if (this.currentUser) {
      this.router.navigate(['/users', this.currentUser.id, 'albums']);
    }}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]]
    });
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.userService.login(this.f.userName.value, this.f.password.value)
      .pipe(first())
      .subscribe(
        () => {
          this.router.navigate(['/users', this.currentUser.id, 'albums']);
        },
        err => {
          this.error = err.status === 400 ? err.error.message : "Unknown error!";         
          this.loading = false;
        });
  }
}
