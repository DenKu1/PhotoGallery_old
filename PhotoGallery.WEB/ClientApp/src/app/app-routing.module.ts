import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';

import { AlbumComponent } from './components/album/album.component';
import { PhotoComponent } from './components/photo/photo.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';

const routes: Routes = [
  {
    path: 'register',
    component: UserRegisterComponent
  },
  {
    path: 'albums',
    component: AlbumComponent
  },
  {
    path: 'photos/:id',
    component: PhotoComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
