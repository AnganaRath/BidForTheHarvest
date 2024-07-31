import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseService {

  protected Bid_For_The_Harvest_URL : string = environment.apiUrl;
  constructor() { }
}
