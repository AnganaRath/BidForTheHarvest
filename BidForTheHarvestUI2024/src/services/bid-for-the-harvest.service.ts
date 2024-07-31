import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { User } from 'src/app/Model/product/user.model';
import { Observable } from 'rxjs';
import { BaseService } from './base.service';
import { Product } from 'src/app/Model/product/product.model';

@Injectable({
  providedIn: 'root'
})
export class BidForTheHarvestService extends BaseService {
  userData: User = new User();
  productDetails: Product = new Product();

  constructor(private http: HttpClient) { 
    super();
  }

  register(user: User): Observable<any[]> {
    user.UserId = "123e4567-e89b-12d3-a456-426614174000";
    return this.http.post<any[]>(`${this.Bid_For_The_Harvest_URL}/UserRegistration`, user);
  }

  login(user: User) {
    user.UserId = "123e4567-e89b-12d3-a456-426614174000";
    // this.setToken('abcdefghijklmnopqrstuvwxyzdgdfg_gdgsdds');
    return this.http.post(this.Bid_For_The_Harvest_URL + '/UserLogIn', user);
  }

  uploadImage(image: FormData): Observable<any> {
    return this.http.post<any>(`${this.Bid_For_The_Harvest_URL}/UploadImage`, image);

  }

  addProductDetails(product: any): Observable<any> {
    const today = new Date().toISOString().split('T')[0]; // Ensure date is in YYYY-MM-DD format
    product.ProductId = "167e4567-e89b-12d3-a456-426614174000";
    product.UserId = "923f3567-e89b-12d3-a456-426614174000"; // Note corrected 'f' instead of 'r'
    //product.ProductImageId = "https://static.toiimg.com/thumb/105672842.cms?width=680&height=512&imgsize=156550";
    product.ProductAddDate = today;
    product.ProductExpireDate = today;
    return this.http.post<any>(`${this.Bid_For_The_Harvest_URL}/AddProductDetails`, product);
  }
  

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.Bid_For_The_Harvest_URL}/GetProducts`);
  }

  addBidding(bidding: any): Observable<any> {
    return this.http.post<any>(`${this.Bid_For_The_Harvest_URL}/AddBidding`, bidding);
  }
 
  getBidCount(productId: string): Observable<number> {
    return this.http.get<number>(`${this.Bid_For_The_Harvest_URL}/GetBidCount/${productId}`);
  }
}
