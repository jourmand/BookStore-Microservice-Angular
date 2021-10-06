import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscribtionUserComponent } from './subscribtion-user.component';

describe('SubscribtionUserComponent', () => {
  let component: SubscribtionUserComponent;
  let fixture: ComponentFixture<SubscribtionUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubscribtionUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscribtionUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
