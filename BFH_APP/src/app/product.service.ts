import { Injectable } from '@angular/core';
import { Product } from './Model/product.module';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs';
// import { Product } from './Model/product.module';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private products: Product[] = [
    { id: 1, name: 'Cabbage', price: 270, description: 'Fresh vegetables grown by local vegetable farmers', imageUrl: 'https://www.ceylone.lk/wp-content/uploads/2022/07/Pic915005.jpg', currentBid: 280 },
    { id: 2, name: 'Tomatoes', price: 225, description: 'Fresh vegetables grown by local vegetable farmers', imageUrl: 'https://www.ceylone.lk/wp-content/uploads/2022/07/2d6e8f78-181a-40c7-adbc-4822789e5bf5.jpg', currentBid: 230 },
    { id: 3, name: 'Beetroot', price: 215, description: 'Fresh vegetables grown by local vegetable farmers', imageUrl: 'https://www.ceylone.lk/wp-content/uploads/2022/07/Pic915002.jpg', currentBid: 220 },
    { id: 4, name: 'Batana', price: 290, description: 'Fresh vegetables grown by local vegetable farmers', imageUrl: 'https://www.ceylone.lk/wp-content/uploads/2022/07/Pic914004.jpg', currentBid: 300 },

  ];

  constructor() { }

  getProducts(): Observable<Product[]> {
    return of(this.products);
  }

  updateBid(productId: number, bid: number): void {
    const product = this.products.find(p => p.id === productId);
    if (product) {
      product.currentBid = bid;
    }
  }
}
