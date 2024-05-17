import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  password: string = '';
  lastName: string = '';
  firstName: string = '';
  email: string = '';
  confirmPassword:string = '';
  userType:string = '';
  phoneNumber:string = '';

  constructor() { }

  ngOnInit(): void {
  }

}
