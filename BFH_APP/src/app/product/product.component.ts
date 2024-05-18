import { Component, OnInit } from '@angular/core';

import { ProductService } from '../product.service';
import { $ } from 'protractor';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent  {

  products: any[] = [];


  constructor() { }

  addProduct() {
    // Logic to add product
    const productName = (document.getElementById('productName') as HTMLInputElement).value;
    const productPrice = (document.getElementById('productPrice') as HTMLInputElement).value;
    const productDescription = (document.getElementById('productDescription') as HTMLInputElement).value;

    // Example: Push new product to products array
    this.products.push({
      name: productName,
      price: productPrice,
      description: productDescription
    });

    // Clear form inputs
    (document.getElementById('productName') as HTMLInputElement).value = '';
    (document.getElementById('productPrice') as HTMLInputElement).value = '';
    (document.getElementById('productDescription') as HTMLInputElement).value = '';

    // Close modal after adding product
    $('#addProductModal').modal('hide');
  }
}