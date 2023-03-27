import React, { Component } from 'react';
import { RedditWindow } from './RedditWindow';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (<RedditWindow />);
  }
}
