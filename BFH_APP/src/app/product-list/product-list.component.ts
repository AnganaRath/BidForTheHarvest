import { Component, OnInit } from '@angular/core';
import { ProductService } from '../product.service';
// import { CartService } from '../cart.service';
import { Router } from '@angular/router';
import { Product } from '../Model/product.module';
// import { Product } from '../models/product/product.module';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  
  products: Product[] = [];
  newBids: { [key: number]: number } = {}; // Track new bids

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getProducts().subscribe((products) => {
      this.products = products;
    });
  }

  placeBid(productId: number): void {
    const bid = this.newBids[productId];
    if (bid) {
      this.productService.updateBid(productId, bid);
    }
  }
}