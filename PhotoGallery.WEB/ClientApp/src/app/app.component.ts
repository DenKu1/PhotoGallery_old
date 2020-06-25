import { Component } from '@angular/core';
import { UserService } from './services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  /*constructor(userService: UserService, router: Router) {
    let userId: number;

    userService.currentUser.subscribe(x => userId = x.id);

    router.navigate(['/users', userId, 'albums']);
  }*/
}
