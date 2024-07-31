import { Component, OnInit } from '@angular/core';
import { BidForTheHarvestService } from 'src/services/bid-for-the-harvest.service';

@Component({
  selector: 'app-seller-interface',
  templateUrl: './seller-interface.component.html',
  styleUrls: ['./seller-interface.component.css']
})
export class SellerInterfaceComponent {

  product = {
    ProductId: '',
    UserId:'',
    ProductName: '',
    ProductAddDate: '',
    ProductExpireDate: '',
    ProductImageId: '',
    ProductPrice: 0,
    Productdescription:''
    // BiddingId:''
  };
  selectedFile: File = null;
  imagePreview: string | ArrayBuffer = null;

  constructor(private harvestService: BidForTheHarvestService) {}

  onFileSelected(event: any) {
  const file: File = event.target.files[0];
  if (file) {
    this.selectedFile = file;

    // For image preview
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result as string;
    };
    reader.readAsDataURL(file);
  }
}

// onSubmit() {
//   // Now save the vegetable details to the database
//   this.harvestService.addProductDetails(this.product).subscribe(response => {
//     console.log('Vegetable added', response);

//     // Optionally, reset the form
//     this.product = {
//       ProductId: '',
//       UserId:'',
//       ProductName: '',
//       ProductAddDate: '',
//       ProductExpireDate: '',
//       ProductImageId: '',
//       ProductPrice: 0,
//       Productdescription:''
//       // BiddingId: ''
//     };
//     this.imagePreview = null;
//     this.selectedFile = null;
//   }, error => {
//     console.error('Error adding product:', error);
//   });
// }

onSubmit() {
  if (this.selectedFile) {
    const formData = new FormData();
    formData.append('image', this.selectedFile, this.selectedFile.name);

    this.harvestService.uploadImage(formData).subscribe(response => {
      this.product.ProductImageId = response.filePath;
      // Now save the vegetable details with the image URL to the database
      this.harvestService.addProductDetails(this.product).subscribe(response => {
        console.log('Vegetable added', response);
        // Optionally, reset the form
        this.product = {
          ProductId: '',
          UserId: '',
          ProductName: '',
          ProductAddDate: '',
          ProductExpireDate: '',
          ProductImageId: '',
          ProductPrice: 0,
          Productdescription: ''
        };
        this.imagePreview = null;
      });
    }, error => {
      console.error('Error uploading image', error);
    });
  }
}
}