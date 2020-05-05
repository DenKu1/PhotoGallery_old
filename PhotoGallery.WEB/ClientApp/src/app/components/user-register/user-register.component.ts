import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {

  constructor(public authService: AuthService, private router: Router)
  {
  }

  ngOnInit() {
    this.authService.signUpForm.reset();
  }

  onSubmit() {
    this.authService.signUp().subscribe(
      res => {
        console.log("OK!");
        this.authService.signUpForm.reset();
        this.router.navigate(['']);
      },
      err => {
        console.log("Error!!1")
      });
  }
}
