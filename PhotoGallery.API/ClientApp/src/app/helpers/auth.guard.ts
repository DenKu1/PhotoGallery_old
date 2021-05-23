import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { UserService } from '../services/user.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

          const currentUser = this.userService.currentUserValue;

          if (currentUser) {
              // check if route is restricted by role
              if (route.data.roles && !route.data.roles.filter(value => currentUser.roles.includes(value)).length) {
                // role not authorised so redirect to home page
                this.router.navigate(['/']);
                return false;
              }
              // logged in so return true
              return true;
          }

          // not logged in so redirect to login page
          this.router.navigate(['/users/login']);
          return false;
    }
}
