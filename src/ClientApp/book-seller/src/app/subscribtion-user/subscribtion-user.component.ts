import { Component, OnInit } from '@angular/core';
import { BookService } from '../_services/book.service';

@Component({
  selector: 'app-subscribtion-user',
  templateUrl: './subscribtion-user.component.html',
  styleUrls: ['./subscribtion-user.component.css']
})
export class SubscribtionUserComponent implements OnInit {
  content?: any;

  constructor(private bookService: BookService) { }

  ngOnInit(): void {
    this.bookService.getSubscribeBooks().subscribe(
      data => {
        this.content = data;
      },
      err => {
        this.content = JSON.parse(err.error).message;
      }
    );
  }

  public unsubscribe(id: string): void {
    this.bookService.unsubscribeBook(id).subscribe(
      data => {
        window.alert("Book Unsubscribed Successfully...");
        window.location.reload();
      },
      err => {
        this.content = JSON.parse(err.error).message;
      }
    );
  }
}
