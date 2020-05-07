import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';

import { AlbumComponent } from './components/album/album.component';
import { PhotoComponent } from './components/photo/photo.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserLoginComponent } from './components/user-login/user-login.component';

import { AuthGuard } from './helpers/auth.guard';

const routes: Routes = [
  {
    path: 'register',
    component: UserRegisterComponent
  },
  {
    path: 'login',
    component: UserLoginComponent
  },
  {
    path: 'albums',
    component: AlbumComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'photos/:id',
    component: PhotoComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: 'albums',
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
