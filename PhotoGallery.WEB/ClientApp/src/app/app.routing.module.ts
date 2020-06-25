import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AlbumComponent } from './components/album/album.component';
import { PhotoComponent } from './components/photo/photo.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserLoginComponent } from './components/user-login/user-login.component';

import { AuthGuard } from './helpers/auth.guard';

const routes: Routes = [
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
    canActivate: [AuthGuard]
  },
  {
    path: 'albums/:id/photos',
    component: PhotoComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    component: AppComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule
{
}
