// src/app/product.ts
export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  imageUrl: string;
  currentBid: number; // Add this line
}
