import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Customer {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}

interface Account {
  id: string;
  customerId: string;
  currency: string;
  balance: number;
  createdAt: string;
  transactions: Transaction[];
  customer: Customer;
}

interface Transaction {
  id: string;
  accountId: string;
  amount: number;
  type: string;
  timestamp: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'MicroBank';
  customers: Customer[] = [];
  accounts: Account[] = [];
  selectedCustomer: Customer | null = null;
  newCustomer: Partial<Customer> = {};
  transaction: { accountId: string; amount: number } = { accountId: '', amount: 0 };
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

  selectCustomer(customer: Customer): void {
    this.selectedCustomer = customer;
    this.loadAccounts(customer.id);
  }

  loadAccounts(customerId: string): void {
    this.http.get<Account[]>(`http://localhost:7000/accounts/customer/${customerId}`)
      .subscribe({
        next: data => this.accounts = data,
        error: err => this.error = err.message
      });
  }

  deposit(): void {
    if (!this.transaction.accountId || this.transaction.amount <= 0 || !this.selectedCustomer) return;
    const request = {
      accountId: this.transaction.accountId,
      amount: this.transaction.amount,
      customerId: this.selectedCustomer.id,
      firstName: this.selectedCustomer.firstName,
      lastName: this.selectedCustomer.lastName,
      email: this.selectedCustomer.email
    };
    this.http.post<Account>('http://localhost:7000/accounts/deposit', request)
      .subscribe({
        next: () => {
          this.transaction = { accountId: '', amount: 0 };
          this.loadAccounts(this.selectedCustomer!.id);
        },
        error: err => this.error = err.message
      });
  }

  withdraw(): void {
    if (!this.transaction.accountId || this.transaction.amount <= 0 || !this.selectedCustomer) return;
    const request = {
      accountId: this.transaction.accountId,
      amount: this.transaction.amount,
      customerId: this.selectedCustomer.id,
      firstName: this.selectedCustomer.firstName,
      lastName: this.selectedCustomer.lastName,
      email: this.selectedCustomer.email
    };
    this.http.post<Account>('http://localhost:7000/accounts/withdraw', request)
      .subscribe({
        next: () => {
          this.transaction = { accountId: '', amount: 0 };
          this.loadAccounts(this.selectedCustomer!.id);
        },
        error: err => this.error = err.message
      });
  }

  deleteAccount(accountId: string): void {
    if (!this.selectedCustomer) return;
    this.http.delete(`http://localhost:7000/accounts/${accountId}`)
      .subscribe({
        next: () => this.loadAccounts(this.selectedCustomer!.id),
        error: err => this.error = err.message
      });
  }
}
