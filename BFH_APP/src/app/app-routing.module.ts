import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { LayoutComponent } from './layout/layout.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductComponent } from './product/product.component';

const routes: Routes = [
  { path: 'logIn', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {path: '', redirectTo: 'logIn', pathMatch: 'full' },
  {path: '', component: LayoutComponent,
   children: [
    { path: 'product', component: ProductComponent},
    { path: 'productList', component: ProductListComponent}
  ]
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
