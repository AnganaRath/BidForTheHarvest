import { Component } from '@angular/core';
import { User } from '../Model/product/user.model';
import { BidForTheHarvestService } from 'src/services/bid-for-the-harvest.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user: User = new User(); 
  submitted = false;

  constructor(private bidForTheHarvestService: BidForTheHarvestService,private router: Router) {}

  onSubmit() {
    this.submitted = true;
    if (!this.isFormValid()) {
      return;
      
    }

    // Call service method to register user
    this.bidForTheHarvestService.register(this.user).subscribe(
      response => {
        
        console.log('User registered successfully', response);
        this.router.navigateByUrl('/');
        
      },
      error => {
        console.error('Error registering user', error);

      }
    );
  }

  isFormValid(): boolean {
    
    return true; 
  }
}
