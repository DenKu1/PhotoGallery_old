import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';

import { AlbumComponent } from './components/album/album.component';
import { PhotoComponent } from './components/photo/photo.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { HomeComponent } from './components/home/home.component';

import { AuthGuard } from './helpers/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
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
    data: { roles: ["user"] }
  },
  {
    path: 'albums/:id/photos',
    component: PhotoComponent,
    canActivate: [AuthGuard],
    data: { roles: ["user"] }
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
