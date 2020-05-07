import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {

  registerForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error: string = '';

  get f() { return this.registerForm.controls; }

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService
  ) {}  

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      UserName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      Email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      Password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      ConfirmPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]]
    },
    {
      validator: this.mustMatch('Password', 'ConfirmPassword')
    });
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    this.userService.register(this.f.UserName.value, this.f.Email.value, this.f.Password.value)
      .pipe(first())
      .subscribe(
        () => {
          this.router.navigate(['login']);
        },
        error => {
          this.error = error.status === 400 ? error.error : "Unknown error!";
          this.loading = false;
        });
  }

  mustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {

      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors.mustmatch) {
        // return if another validator has already found an error on the matchingControl
        return;
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustmatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    }
  }
}
