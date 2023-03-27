import React, { Component } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { Button, ButtonGroup, Row, Col } from "reactstrap";
import PostCount from "./PostCount";
import PostTitles from "./PostTitles";
import PostAuthors from "./PostAuthors";
import ControlButtons from "./ControlButtons";

export class RedditWindow extends Component {
  constructor(props) {
    super(props);
    this.state = { postCount: 0, postTitles: [], postAuthors: [] };
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/hubs/posts")
      .withAutomaticReconnect()
      .build();
    this.state.connection = newConnection;
    this.handleFlush = this.handleFlush.bind(this);
  }

  componentDidMount() {
    if (this.state.connection) {
      this.state.connection
        .start()
        .then(() => {
          console.log("Connected!");

          this.state.connection.on("SendPostCount", (message) => {
            this.setState({ postCount: message });
          });

          this.state.connection.on("SendPostTitles", (message) => {
            this.setState({ postTitles: message });
            //this.setState({ postTitles: this.state.postTitles.concat(message) });
          });

          this.state.connection.on("SendPostAuthors", (message) => {
            this.setState({ postAuthors: message });
            // Some pulls are > 1000 in length, so replacing current array instead
            // of concatenating is fine for a demo.
            //this.setState({ postAuthors: this.state.postAuthors.concat(message) });
          });
        })
        .catch((e) => console.log("Connection failed: ", e));
    }
  }

  handleFlush(){
    this.setState({ postCount: 0, postTitles: [], postAuthors: [] });
  }

  render() {
    return (
      <div>
        <Row>
          <PostTitles postTitles={this.state.postTitles} />
          <PostAuthors postAuthors={this.state.postAuthors} />
          <PostCount postCount={this.state.postCount} />
        </Row>
        <Row>
          <ControlButtons reset={this.handleFlush} />
        </Row>
      </div>
    );
  }
}
