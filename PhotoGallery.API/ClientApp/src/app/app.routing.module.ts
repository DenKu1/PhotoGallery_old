import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';

import { AlbumComponent } from './components/album/album.component';
import { PhotoComponent } from './components/photo/photo.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { HomeComponent } from './components/home/home.component';

import { AuthGuard } from './helpers/auth.guard';
import { UserManagerComponent } from './components/user-manager/user-manager.component';
import { UserProfileComponent } from "./components/user-profile/user-profile.component";

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'profile',
    component: UserProfileComponent
  },
  {
    path: 'users/register',
    component: UserRegisterComponent
  },
  {
    path: 'users/login',
    component: UserLoginComponent
  },
  {
    path: 'users/:id/albums',
    component: AlbumComponent,
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    canActivate: [AuthGuard],
    data: { roles: ["User"] }
  },
  {
    path: 'albums/:id/photos',
    component: PhotoComponent,
    canActivate: [AuthGuard],
    data: { roles: ["User"] }
  },
  {
    path: 'admin',
    component: UserManagerComponent,
    canActivate: [AuthGuard],
    data: { roles: ["Admin"] }
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule
{
}
