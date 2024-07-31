import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProductComponent } from './product/product.component';
import { ProductListComponent } from './product-list/product-list.component';
import { SellerInterfaceComponent } from './seller-interface/seller-interface.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  // {path: '', redirectTo: 'logIn', pathMatch: 'full' },
  {path: '', component: LayoutComponent,
   children: [
  { path: 'productList', component: ProductListComponent},
  { path: 'product', component: ProductComponent},
  { path: 'sellerInterface', component: SellerInterfaceComponent}
  
  ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
