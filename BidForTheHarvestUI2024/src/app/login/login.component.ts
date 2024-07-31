import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BidForTheHarvestService } from 'src/services/bid-for-the-harvest.service';
import { User } from '../Model/product/user.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  
  user: User = new User(); 

  constructor(private bidForTheHarvestService: BidForTheHarvestService,private router: Router) { }

  ngOnInit(): void {
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.resetForm();
    this.user = {
    UserId :  '',
    firstName: '',
    lastName: '',
    userPhoneNumber: '',
    email: '',
    userType: 0,
    password: ''
    }
  }

  OnSubmit() {
   
    this.bidForTheHarvestService.login(this.user).subscribe(
      res => {
        this.user.firstName = res["firstName"];
        this.user.password = res["password"]
        this.user.UserId = res["UserId"];
        this.user.userType = res["userType"]
        this.router.navigateByUrl('/productList');
        
      },
      err => {
        if (err.status == 400) {
          alert ("Hi Invalid First name Or Password")
        }
        else {
         
        }
      });
    this.resetForm();
  }
}


