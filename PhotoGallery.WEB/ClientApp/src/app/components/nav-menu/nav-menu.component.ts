import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  currentUser : User;

  constructor(
    private router: Router,
    private userService: UserService
  ) {
    this.userService.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit(): void {
  }

  onLogout() {
    this.userService.logout();
    this.router.navigate(['/login']);
  }
}
