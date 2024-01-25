import { useState } from "react";
import BikeList from "./BikeList";
import BikeDetails from "./BikeDetails";

export default function Bikes() {
  const [detailsBikeId, setDetailsBikeId] = useState(null);

  return (
    <div className="container">
      <div className="row">
        <div className="col-sm-8">
          {/* passes setter to the list of bikes */}
          <BikeList setDetailsBikeId={setDetailsBikeId} />
        </div>
        <div className="col-sm-4">
          {/* passes chosen bikeId to details compo */}
          <BikeDetails detailsBikeId={detailsBikeId} />
        </div>
      </div>
    </div>
  );
}
