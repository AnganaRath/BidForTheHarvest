import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs';
import { Product } from './Model/product/product.model';

// import { Product } from './Model/product.module';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private products: Product[] = [
    { ProductId: '5bca47d5-1230-4c8f-bb4a-d8e1f0b07374', ProductName: 'Cabbage', ProductPrice: 270, Productdescription: 'Fresh vegetables grown by local vegetable farmers', ProductImageId: 'https://www.ceylone.lk/wp-content/uploads/2022/07/Pic915005.jpg', UserId:'5bca47d5-1230-4c8f-bb4a-d8e1f0b07274',ProductAddDate:'',ProductExpireDate:'' },
    { ProductId: '5bca47d5-1230-4c8f-bb4a-d8e1f0b07344', ProductName: 'Tomatoes', ProductPrice: 225, Productdescription: 'Fresh vegetables grown by local vegetable farmers', ProductImageId: 'https://www.ceylone.lk/wp-content/uploads/2022/07/2d6e8f78-181a-40c7-adbc-4822789e5bf5.jpg',UserId:'5bca47d5-1230-4c8f-bf4a-d8e1f0b07374',ProductAddDate:'',ProductExpireDate:''  },
    { ProductId: '5bca47d5-1230-4c8f-bb4a-d8e1f0b05374', ProductName: 'Beetroot', ProductPrice: 215, Productdescription: 'Fresh vegetables grown by local vegetable farmers', ProductImageId: 'https://www.ceylone.lk/wp-content/uploads/2022/07/Pic915002.jpg',UserId:'5bca47d5-1530-4c8f-bb4a-d8e1f0b07374',ProductAddDate:'',ProductExpireDate:''  },
    { ProductId: '5bca47d5-1230-4c8f-bb4a-d8e1f0b57374', ProductName: 'Batana', ProductPrice: 290, Productdescription: 'Fresh vegetables grown by local vegetable farmers', ProductImageId: 'https://www.ceylone.lk/wp-content/uploads/2022/07/Pic914004.jpg',UserId:'5bca47d5-1230-4d8f-bb4a-d8e1f0b07374',ProductAddDate:'',ProductExpireDate:''  },

  ];

  constructor() { }

  getProducts(): Observable<Product[]> {
    return of(this.products);
  }

  updateBid(productId: string, bid: string): void {
    const product = this.products.find(p => p.ProductId === productId);
    if (product) {
      // product.BiddingId = bid;
    }
  }
}
