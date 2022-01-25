import React, { Component } from 'react';
import { Button } from "reactstrap";

export class FetchItems extends Component<any, any> {
  static displayName = FetchItems.name;

  constructor(props: any) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    void this.populateWeatherData();
  }

  renderForecastsTable(forecasts: any) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
        <tr>
          <th>Id</th>
          <th>Display Name</th>
          <th>Description</th>
          <th>Price</th>
          <th>Type</th>
          <th>Is Available</th>
          <th>Available Since</th>
          <th>Available Before</th>
          <th>Buy it</th>
        </tr>
        </thead>
        <tbody>
        {forecasts.map((forecast: any) =>
          <tr key={forecast.id}>
            <td>{forecast.id}</td>
            <td>{forecast.displayName}</td>
            <td>{forecast.description}</td>
            <td>{forecast.price}</td>
            <td>{forecast.type}</td>
            <td>{forecast.isEnabled ? 'yes' : 'no'}</td>
            <td>{forecast.availableSince}</td>
            <td>{forecast.availableBefore ?? 'not set'}</td>
            <td><Button onClick={async () => await this.makePayment(forecast.id)}>Buy</Button></td>
          </tr>
        )}
        </tbody>
      </table>
    );
  }

  makePayment = async (itemId: any) => {
    const resp = await fetch(`items/${itemId}/order`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        clientInfo: {
          firstName: "Artem",
          secondName: "Stepanov",
          email: "stxima@gmail.com",
          phoneNumber: "89998030386"
        }
      })
    });
    const data = await resp.json();
    if(!data?.url) {
      alert("can't create bill, blyat");
      return;
    }
    window.location.replace(data.url);
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('items/available');
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
}
