import { Component, OnInit } from '@angular/core';
import { BookService } from '../_services/book.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  content?: any;

  constructor(private bookService: BookService) { }

  ngOnInit(): void {
    this.bookService.getAllBook().subscribe(
      data => {
        this.content = data;
      },
      err => {
        this.content = JSON.parse(err.error).message;
      }
    );
  }

  public subscribe(id: string): void {
    this.bookService.subscribeBook(id).subscribe(
      data => {
        window.alert("Book Subscribed Successfully...");
        //window.location.reload();
      },
      err => {
        window.alert(err.error.errors.map((e: { message: string; }) => e.message).join(","));
      }
    );
  }
}
