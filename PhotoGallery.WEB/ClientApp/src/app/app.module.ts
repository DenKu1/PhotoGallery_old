import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { AlbumComponent } from './components/album/album.component';
import { AlbumCreateComponent } from './components/album-create/album-create.component';
import { AlbumGetAllComponent } from './components/album-get-all/album-get-all.component';
import { AlbumGetComponent } from './components/album-get/album-get.component';
import { PhotoComponent } from './components/photo/photo.component';
import { PhotoGetAllComponent } from './components/photo-get-all/photo-get-all.component';
import { PhotoAddComponent } from './components/photo-add/photo-add.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    AlbumComponent,
    AlbumCreateComponent,
    AlbumGetAllComponent,
    AlbumGetComponent,
    PhotoComponent,
    PhotoGetAllComponent,
    PhotoAddComponent,
    UserRegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
