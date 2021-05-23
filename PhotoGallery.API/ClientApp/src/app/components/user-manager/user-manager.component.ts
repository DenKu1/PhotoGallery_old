import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrls: ['./user-manager.component.css']
})
export class UserManagerComponent implements OnInit {

  users: User[];

  constructor(
    private userService: UserService) {
  }

  ngOnInit() {
    this.getUsers();
  }

  getUsers(): void {
    this.userService.getUsers()
      .pipe(first())
      .subscribe(
        users => {
          this.users = users;
        });
  }

  deleteUser(userId: number, index: number)
  {
    this.userService.deleteUser(userId)
      .pipe(first())
      .subscribe(
        () => {
          this.users.splice(index, 1);
        },
        err => {
          console.log("Can`t delete user! Unknown error");
        });
  }
}
