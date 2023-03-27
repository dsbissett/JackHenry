import React from "react";
import { Button, ButtonGroup } from "reactstrap";

const ControlButtons = (props) => {
  const startStreaming = async () =>
    await fetch("/api/reddit", { method: "GET" }).catch((error) =>
      console.log(error)
    );

  const stopStreaming = async () =>
    await fetch("/api/reddit", { method: "POST" }).catch((error) =>
      console.log(error)
    );

  const flushRedis = async () => {
    await fetch("/api/reddit", { method: "DELETE" }).catch((error) =>
      console.log(error)
    );
    
    props.reset();
  };

  return (
    <ButtonGroup>
      <Button color="success" onClick={startStreaming}>
        Start
      </Button>
      <Button color="warning" onClick={stopStreaming}>
        Stop
      </Button>
      <Button color="danger" onClick={flushRedis}>
        Flush
      </Button>
    </ButtonGroup>
  );
};
export default ControlButtons;
