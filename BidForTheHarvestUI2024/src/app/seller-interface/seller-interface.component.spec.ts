import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerInterfaceComponent } from './seller-interface.component';

describe('SellerInterfaceComponent', () => {
  let component: SellerInterfaceComponent;
  let fixture: ComponentFixture<SellerInterfaceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerInterfaceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SellerInterfaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
