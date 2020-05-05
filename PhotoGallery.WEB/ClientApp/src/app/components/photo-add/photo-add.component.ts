import { Component, OnInit } from '@angular/core';

import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-photo-add',
  templateUrl: './photo-add.component.html',
  styleUrls: ['./photo-add.component.css']
})
export class PhotoAddComponent implements OnInit {

  AlbumAddForm: FormGroup;

  constructor() {
    this.AlbumAddForm = new FormGroup
      ({
        "Name": new FormControl("", [
          Validators.required,
          Validators.maxLength(50),
          Validators.pattern("^(?!\s * $).+ $")
        ]),
        "Description": new FormControl("", [
          Validators.required,
          Validators.email
        ])    
      });
  }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.AlbumAddForm);
  }
}
