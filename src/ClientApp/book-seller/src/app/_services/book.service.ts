import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_URL = 'http://localhost:5113/api/1/';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  constructor(private http: HttpClient) { }

  getAllBook(): Observable<any> {
    return this.http.get(API_URL + 'book');
  }

  getSubscribeBooks(): Observable<any> {
    return this.http.get(API_URL + 'subscribe');
  }

  subscribeBook(bookId: string): Observable<any> {
    return this.http.post(API_URL + 'subscribe', {
      bookId,
    });
  }

  unsubscribeBook(bookId: string): Observable<any> {
    return this.http.delete(`${API_URL}subscribe/${bookId}`);
  }
}
