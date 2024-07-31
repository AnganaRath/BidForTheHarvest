import { Component, OnInit } from '@angular/core';
import { ProductService } from '../product.service';
// import { CartService } from '../cart.service';
import { Router } from '@angular/router';
import { Product } from '../Model/product/product.model';
import { BidForTheHarvestService } from 'src/services/bid-for-the-harvest.service';


// import { Product } from '../models/product/product.module';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  

  products: any = [];
  newBids: { [key: string]: number } = {};
  
  // constructor(private router:Router,private productService: ProductService) { }
  constructor(private router:Router,private harvestService: BidForTheHarvestService) { }

  // ngOnInit(): void {
  //   this.harvestService.getProducts().subscribe(
  //     (data: any[]) => {
  //       this.products = data;
  //     },
  //     (error) => {
  //       console.error('Error fetching products', error);
  //     }
  //   );
  // }

  ngOnInit(): void {
    this.harvestService.getProducts().subscribe(
      (data: any[]) => {
        this.products = data.map(product => {
          product.productImageId = this.getImageUrl(product.productImageId);
          return product;
        });
      },
      (error) => {
        console.error('Error fetching products', error);
      }
    );
  }

  getImageUrl(imagePath: string): string {
    const baseUrl = 'http://localhost:62634';
   
    return `${baseUrl}${imagePath}`;
  }

  onImageError(event: Event): void {
    (event.target as HTMLImageElement).src = 'assets/default-image.jpg'; // Path to a default image
  }


  // placeBid(productId: string): void {
  //   const bidAmount = this.newBids[productId];
  //   if (bidAmount) {
  //     // Handle bid placement logic here
  //     console.log(`Bid placed on product ${productId}: Rs.${bidAmount}`);
  //   }
  // }

  placeBid(productId: string,userId:string): void {
    const bidAmount = this.newBids[productId];
    if (bidAmount) {
      this.harvestService.getBidCount(productId).subscribe(
        (existingBidCount: number) => {
      const bid = {
        ProductId: productId,
        UserId: userId, 
        BiddingPrice: bidAmount,
        NoOfBids:  existingBidCount + 1
      };
  
      this.harvestService.addBidding(bid).subscribe(
        response => {
          console.log('Bid placed', response);
          this.newBids[productId] = null;
        },
        error => {
          console.error('Error placing bid', error);
        }
      );
    },
      error => {
        console.error('Error retrieving bid count', error);
    }
);
}
  }
}
