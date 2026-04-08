import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Customer {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'MicroBank';
  customers: Customer[] = [];
  newCustomer: Partial<Customer> = {};
  error = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.http.get<Customer[]>('http://localhost:7000/customers')
      .subscribe({
        next: data => this.customers = data,
        error: err => this.error = err.message
      });
  }

  createCustomer(): void {
    this.http.post<Customer>('http://localhost:7000/customers', this.newCustomer)
      .subscribe({
        next: () => {
          this.newCustomer = {};
          this.loadCustomers();
        },
        error: err => this.error = err.message
      });
  }
}
